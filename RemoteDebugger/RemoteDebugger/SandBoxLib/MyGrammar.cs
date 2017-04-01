using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony;
using Irony.Parsing;

namespace SandBoxLib
{
    public class MyGrammar : Irony.Parsing.Grammar
    {

        public MyGrammar()
        {
            //数字
            var number = new NumberLiteral("number");
            number.DefaultIntTypes = new[] { TypeCode.Int32,
                                          TypeCode.Int64,
                                          NumberLiteral.TypeCodeBigInt};

            number.DefaultFloatType = TypeCode.Double;
            //識別子
            var identifier = new IdentifierTerminal("identifier",".");
            //コメント
            var comment = new CommentTerminal("comment", "//", "\n", "\r");

            //
            //非終端記号を定義
            //

            //var identifier = new NonTerminal("identifier");
            var Expr = new NonTerminal("Expression");
            var Term = new NonTerminal("Term");
            var BinExpr = new NonTerminal("BinaryExpression");
            var BinOp = new NonTerminal("BinaryOperator");
            var CompareExpr = new NonTerminal("CompareExpression");
            var Comparator = new NonTerminal("Comparator");
            var AssignmentStmt = new NonTerminal("AssignmentStatement");
            var AssignmentOp = new NonTerminal("AssignmentOperator");
            var Statement = new NonTerminal("Statement");
            var ProgramLine = new NonTerminal("ProgramLine");
            var Program = new NonTerminal("Program");

            //
            //文法を定義
            //

            //identifier.Rule = identifierAtom + "." + identifierAtom;
            Expr.Rule = Term | "(" + Term + ")" | BinExpr | "(" + BinExpr + ")" | CompareExpr | "(" + CompareExpr + ")";
            Term.Rule = number | identifier;
            BinExpr.Rule = Expr + BinOp + Expr;
            BinOp.Rule = ToTerm("+") | "-" | "*" | "/";

            CompareExpr.Rule = "";
            Comparator.Rule = ToTerm("==") | "!=" | "<" | "<=" | ">" | ">=";

            AssignmentStmt.Rule = identifier + AssignmentOp + Expr;
            AssignmentOp.Rule = ToTerm("=") | "+=" | "-=" | "*=" | "/=";

            Statement.Rule = AssignmentStmt | Expr | Empty;
            ProgramLine.Rule = Statement + ToTerm(";");
            Program.Rule = MakeStarRule(Program, ProgramLine);
            //文法のルートを設定
            this.Root = Program;

            //
            //演算子の優先順位を定義
            //

            RegisterOperators(1, "+", "-");
            RegisterOperators(2, "*", "/");

            this.LanguageFlags = LanguageFlags.NewLineBeforeEOF |
                                 LanguageFlags.SupportsBigInt;

        }

    }
}
