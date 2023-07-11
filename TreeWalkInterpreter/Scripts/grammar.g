﻿program -> declaration* EOF;
declaration -> funcDecl | varDecl | statement;
funDecl -> "fun" function ;
function -> IDENTIFIER "(" parameters? ")" block ;
parameters -> IDENTIFIER ( "," IDENTIFIER )* ;
varDecl -> "var" IDENTIFIER ( "=" expression )? ";";
statement -> exprStatement | forStatement | ifStatement | printStatement | returnStatement | whileStatement | block;
exprStatement -> expression ";";
forStatement -> "for" "(" ( varDecl | exprStmt | ";" ) expression? ";" expression? ")" statement;
ifStatement -> "if" "(" expression ")" statement ( "else" statement )?;
printStatement -> "print" expression ";";
whileStatement -> "while" "(" expression ")" statement;
returnStatement -> "return" expression? ";" ;
block -> "{" declaration* "}";
expression -> assignment;
assignment -> IDENTIFIER "=" assignment | logic_or;
logic_or -> logic_and ( "or" logic_and )*;
logic_and -> equality( "and" equality)*; 
equality -> comparison ( ( "!=" | "==" ) comparison )* ; 
comparison -> term ( ( ">" | ">=" | "<" | "<=" ) term)* ;
term -> factor ( ( "-" | "+" ) factor )* ;
factor -> unary ( ( "/" | "*" ) unary)*;
unary -> ( "!" | "-" ) unary | call ;
call -> primary ( "(" arguments? ")" )* ;
arguments -> expression ( "," expression )* ;
primary -> literal | grouping | IDENTIFIER; 
literal -> NUMBER | STRING| "true" | "false" | "nil" ;
grouping -> "(" expression ")" ;