using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HLRemoting.Scripting
{
    public class MailScriptExecuter
    {
        private Dictionary<string, object> _variables = null;
        private Dictionary<string, object> _modified = null;
        public Dictionary<string, object> Variables
        {
            set
            {
                _variables = value;
                _modified = new Dictionary<string, object>(value);
            }
            get
            {
                return _variables;
            }
        }
        public Dictionary<string, object> ModifiedVariables
        {
            get
            {
                return _modified;
            }
        }

        private readonly Irony.Parsing.Parser _p = new Irony.Parsing.Parser(new MailScriptGrammar());

        public List<object> Execute(string script)
        {
            var t = _p.Parse(script);
            if (0 < t.ParserMessages.Count)
            {
                throw new Exception(t.ParserMessages[0].Message);
            }

            var results = new List<object>();

            var program = t.Root;
            System.Diagnostics.Debug.Assert(program.Term.Name.Equals("Program"));
            for (int i = 0; i < program.ChildNodes.Count; i++)
            {
                var line = program.ChildNodes[i];
                System.Diagnostics.Debug.Assert(line.Term.Name.Equals("ProgramLine"));
                results.Add(ProcessLine(line));
            }

            return results;
        }

        private object ProcessLine(Irony.Parsing.ParseTreeNode line)
        {
            System.Diagnostics.Debug.Assert(2 == line.ChildNodes.Count);
            System.Diagnostics.Debug.Assert(line.ChildNodes[1].Term.Name.Equals(";"));
            return ProcessStatement(line.ChildNodes[0]);
        }

        private object ProcessStatement(Irony.Parsing.ParseTreeNode item)
        {
            System.Diagnostics.Debug.Assert(item.Term.Name.Equals("Statement"));
            System.Diagnostics.Debug.Assert(1 == item.ChildNodes.Count);
            object result = null;
            switch (item.ChildNodes[0].Term.Name)
            {
                case "Expression":
                    result = ProcessExpression(item.ChildNodes[0]);
                    break;
                case "AssignmentStatement":
                    result = ProcessAssignmentStatement(item.ChildNodes[0]);
                    break;
                default:
                    throw new Exception(string.Format("NotSupportedItem {0}", item.ToString()));
            }
            return result;
        }

        private object ProcessExpression(Irony.Parsing.ParseTreeNode item)
        {
            System.Diagnostics.Debug.Assert(item.Term.Name.Equals("Expression"));
            System.Diagnostics.Debug.Assert(1 == item.ChildNodes.Count || 3 == item.ChildNodes.Count);
            var target = item.ChildNodes[0];

            object result = null;
            var isNegative = false;
            if (target.Term.Name.Equals("ExpressionP"))
            {
            }
            else
            {
                // ExpressionNはPより階層がひとつ深いので注意
                isNegative = true;
                target = target.ChildNodes[1];
            }

            var valueTarget = target.ChildNodes[0];
            if (3 == target.ChildNodes.Count)
            {
                System.Diagnostics.Debug.Assert(target.ChildNodes[0].Term.Name.Equals("("));
                System.Diagnostics.Debug.Assert(target.ChildNodes[2].Term.Name.Equals(")"));
                valueTarget = target.ChildNodes[1];
            }

            switch (valueTarget.Term.Name)
            {
                case "Term":
                    result = ProcessTerm(valueTarget);
                    break;

                case "BinaryExpression":
                    result = ProcessBinaryExpression(valueTarget);
                    break;

                case "CompareExpression":
                    result = ProcessCompareExpression(valueTarget);
                    break;

                default:
                    throw new Exception(string.Format("NotSupportedItem {0}", item.ToString()));
            }
            if (isNegative)
            {
                if (0 == (int)result)
                {
                    result = 1;
                }
                else
                {
                    result = 0;
                }
            }
            return result;
        }

        private object ProcessTerm(Irony.Parsing.ParseTreeNode item)
        {
            System.Diagnostics.Debug.Assert(item.Term.Name.Equals("Term"));
            System.Diagnostics.Debug.Assert(1 == item.ChildNodes.Count);

            object result = null;
            switch (item.ChildNodes[0].Term.Name)
            {
                case "number":
                    result = item.ChildNodes[0].Token.Value;
                    break;
                case "NegativeNumber":
                    result = -(int)item.ChildNodes[0].ChildNodes[1].Token.Value;
                    break;
                case "identifier":
                    var varName = item.ChildNodes[0].Token.Value.ToString();
                    if (null == _modified) throw new Exception(string.Format("{0}が定義されていません",varName));
                    if (! _modified.ContainsKey(varName)) throw new Exception(string.Format("{0}が定義されていません",varName));
                    result = _modified[varName];
                    break;
                default:
                    throw new Exception(string.Format("NotSupportedItem {0}", item.ToString()));
            }
            return result;
        }

        private object ProcessBinaryExpression(Irony.Parsing.ParseTreeNode item)
        {
            System.Diagnostics.Debug.Assert(item.Term.Name.Equals("BinaryExpression"));
            System.Diagnostics.Debug.Assert(3 == item.ChildNodes.Count);

            var left = ProcessExpression(item.ChildNodes[0]);
            var right = ProcessExpression(item.ChildNodes[2]);
            object result = null;
            switch (item.ChildNodes[1].ChildNodes[0].Token.Value.ToString())
            {
                case "+":
                    result = (int)(left) + (int)(right);
                    break;
                case "-":
                    result = (int)(left) - (int)(right);
                    break;
                case "/":
                    result = (int)((int)(left) / (int)(right));
                    break;
                case "*":
                    result = (int)(left) * (int)(right);
                    break;
                default:
                    throw new Exception(string.Format("NotSupportedItem {0}", item.ToString()));
            }
            return result;
        }

        private object ProcessCompareExpression(Irony.Parsing.ParseTreeNode item)
        {
            System.Diagnostics.Debug.Assert(item.Term.Name.Equals("CompareExpression"));
            System.Diagnostics.Debug.Assert(3 == item.ChildNodes.Count);

            var left = ProcessExpression(item.ChildNodes[0]);
            var right = ProcessExpression(item.ChildNodes[2]);
            object result = null;
            switch (item.ChildNodes[1].ChildNodes[0].Token.Value.ToString())
            {
                case ">":
                    result = (int)(left) > (int)(right) ? 1 : 0;
                    break;
                case "<":
                    result = (int)(left) < (int)(right) ? 1 : 0;
                    break;
                case ">=":
                    result = (int)(left) >= (int)(right) ? 1 : 0;
                    break;
                case "<=":
                    result = (int)(left) <= (int)(right) ? 1 : 0;
                    break;
                case "==":
                    result = (int)(left) == (int)(right) ? 1 : 0;
                    break;
                case "!=":
                    result = (int)(left) != (int)(right) ? 1 : 0;
                    break;
                case "&&":
                    result = (int)(left) != 0 && (int)(right) != 0 ? 1 : 0;
                    break;
                case "||":
                    result = (int)(left) != 0 || (int)(right) != 0 ? 1 : 0;
                    break;
                case "^^":
                    result = (int)(left) != 0 ^ (int)(right) != 0 ? 1 : 0;
                    break;
                default:
                    throw new Exception(string.Format("NotSupportedItem {0}", item.ToString()));
            }
            return result;
        }

        private object ProcessAssignmentStatement(Irony.Parsing.ParseTreeNode item)
        {
            System.Diagnostics.Debug.Assert(item.Term.Name.Equals("AssignmentStatement"));
            System.Diagnostics.Debug.Assert(3 == item.ChildNodes.Count);
            var varName = item.ChildNodes[0].Token.Value.ToString();
            if (null == _modified) throw new Exception(string.Format("{0}が定義されていません", varName));
            if (!_modified.ContainsKey(varName)) throw new Exception(string.Format("{0}が定義されていません", varName));
            var right = ProcessExpression(item.ChildNodes[2]);
            object result = null;
            switch (item.ChildNodes[1].ChildNodes[0].Token.Value.ToString())
            {
                case "=":
                    _modified[varName] = (int)right;
                    break;
                case "+=":
                    _modified[varName] = (int)_modified[varName] + (int)right;
                    break;
                case "-=":
                    _modified[varName] = (int)_modified[varName] - (int)right;
                    break;
                case "/=":
                    _modified[varName] = (int)((int)_modified[varName] / (int)right);
                    break;
                case "*=":
                    _modified[varName] = (int)_modified[varName] * (int)right;
                    break;
                default:
                    throw new Exception(string.Format("NotSupportedItem {0}", item.ToString()));
            }
            result = string.Format("{0} {1} {2}", varName, item.ChildNodes[1].ChildNodes[0].Token.Value, right);
            return result;
        }
    }
    }