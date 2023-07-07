﻿program -> declaration* EOF;
declaration -> varDecl | statement;
varDecl -> "var" IDENTIFIER ( "=" expression )? ";";
statement -> exprStatement | printStatement | block;
exprStatement -> expression ";";
printStatement -> "print" expression ";";
block -> "{" declaration* "}";
expression -> assignment;
assignment -> IDENTIFIER "=" assignment | equality; 
equality -> comparison ( ( "!=" | "==" ) comparison )* ; 
comparison -> term ( ( ">" | ">=" | "<" | "<=" ) term)* ;
term -> factor ( ( "-" | "+" ) factor )* ;
factor -> unary ( ( "/" | "*" ) unary)*;
unary -> ( "!" | "-" ) unary | primary ;
primary -> literal | grouping | IDENTIFIER; 
literal -> NUMBER | STRING| "true" | "false" | "nil" ;
grouping -> "(" expression ")" ;