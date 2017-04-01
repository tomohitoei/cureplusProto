using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Parsing;

namespace HLRemoting.Scripting
{
    public class MailScriptGrammar:Grammar
    {
        public MailScriptGrammar()
        {
            //数字
            var number = new NumberLiteral("number");
            number.DefaultIntTypes = new[] { TypeCode.Int32,
                                          TypeCode.Int64,
                                          NumberLiteral.TypeCodeBigInt};
            number.DefaultFloatType = TypeCode.Double;
            //NumberLiteral number = TerminalFactory.CreateCSharpNumber("number");

            // TODO : 変数名に日本語を許容する(Ironyだと面倒？
            //識別子
            var identifier = new IdentifierTerminal("identifier", ".");
            
            //コメント
            var comment = new CommentTerminal("comment", "//", "\n", "\r");

            //
            //非終端記号を定義
            //

            //var identifier = new NonTerminal("identifier");
            var Expr = new NonTerminal("Expression");
            var ExprP = new NonTerminal("ExpressionP");
            var ExprN = new NonTerminal("ExpressionN");
            var Term = new NonTerminal("Term");
            var NotTerm = new NonTerminal("NotTerm");
            var NNumber = new NonTerminal("NegativeNumber");
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
            Expr.Rule = ExprP | ExprN;
            ExprP.Rule = Term | "(" + Term + ")" | BinExpr | "(" + BinExpr + ")" | CompareExpr | "(" + CompareExpr + ")";
            ExprN.Rule = "!" + ExprP;
            NNumber.Rule = "-" + number;
            Term.Rule = number | NNumber | identifier;
            BinExpr.Rule = Expr + BinOp + Expr;
            BinOp.Rule = ToTerm("+") | "-" | "*" | "/";

            CompareExpr.Rule = Expr + Comparator + Expr;
            Comparator.Rule = ToTerm("==") | "!=" | "<" | "<=" | ">" | ">=" | "&&" | "||" | "^^";

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
            RegisterOperators(3, "==", "!=", "<", "<=", ">", ">=", "&&", "||", "^^");

            this.LanguageFlags = LanguageFlags.NewLineBeforeEOF |
                                 LanguageFlags.SupportsBigInt;
        }

    }
}
