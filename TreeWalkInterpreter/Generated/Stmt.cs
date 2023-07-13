// This file is generated. Do not edit!

using Campfire.TreeWalkInterpreter;

public abstract class Stmt {

  public interface Visitor<T> {
    T VisitBlockStmt(Block stmt);
    T VisitClassStmt(Class stmt);
    T VisitExpressionStmt(Expression stmt);
    T VisitFunctionStmt(Function stmt);
    T VisitIfStmt(If stmt);
    T VisitPrintStmt(Print stmt);
    T VisitReturnStatementStmt(ReturnStatement stmt);
    T VisitVarStmt(Var stmt);
    T VisitWhileStmt(While stmt);
  }

  public abstract T Accept<T>(Visitor<T> visitor);
}

 public class Block : Stmt {
    public readonly List<Stmt> statements;

    public Block(List<Stmt> statements){
      this.statements = statements;
    }

    public override T Accept<T>(Visitor<T> visitor){
      return visitor.VisitBlockStmt(this);
    }
  }

 public class Class : Stmt {
    public readonly Token name;
    public readonly Variable superclass;
    public readonly List<Function> methods;

    public Class(Token name, Variable superclass, List<Function> methods){
      this.name = name;
      this.superclass = superclass;
      this.methods = methods;
    }

    public override T Accept<T>(Visitor<T> visitor){
      return visitor.VisitClassStmt(this);
    }
  }

 public class Expression : Stmt {
    public readonly Expr expression;

    public Expression(Expr expression){
      this.expression = expression;
    }

    public override T Accept<T>(Visitor<T> visitor){
      return visitor.VisitExpressionStmt(this);
    }
  }

 public class Function : Stmt {
    public readonly Token name;
    public readonly List<Token> parameters;
    public readonly List<Stmt> body;

    public Function(Token name, List<Token> parameters, List<Stmt> body){
      this.name = name;
      this.parameters = parameters;
      this.body = body;
    }

    public override T Accept<T>(Visitor<T> visitor){
      return visitor.VisitFunctionStmt(this);
    }
  }

 public class If : Stmt {
    public readonly Expr condition;
    public readonly Stmt thenBranch;
    public readonly Stmt elseBranch;

    public If(Expr condition, Stmt thenBranch, Stmt elseBranch){
      this.condition = condition;
      this.thenBranch = thenBranch;
      this.elseBranch = elseBranch;
    }

    public override T Accept<T>(Visitor<T> visitor){
      return visitor.VisitIfStmt(this);
    }
  }

 public class Print : Stmt {
    public readonly Expr expression;

    public Print(Expr expression){
      this.expression = expression;
    }

    public override T Accept<T>(Visitor<T> visitor){
      return visitor.VisitPrintStmt(this);
    }
  }

 public class ReturnStatement : Stmt {
    public readonly Token keyword;
    public readonly Expr value;

    public ReturnStatement(Token keyword, Expr value){
      this.keyword = keyword;
      this.value = value;
    }

    public override T Accept<T>(Visitor<T> visitor){
      return visitor.VisitReturnStatementStmt(this);
    }
  }

 public class Var : Stmt {
    public readonly Token name;
    public readonly Expr initializer;

    public Var(Token name, Expr initializer){
      this.name = name;
      this.initializer = initializer;
    }

    public override T Accept<T>(Visitor<T> visitor){
      return visitor.VisitVarStmt(this);
    }
  }

 public class While : Stmt {
    public readonly Expr condition;
    public readonly Stmt body;

    public While(Expr condition, Stmt body){
      this.condition = condition;
      this.body = body;
    }

    public override T Accept<T>(Visitor<T> visitor){
      return visitor.VisitWhileStmt(this);
    }
  }

