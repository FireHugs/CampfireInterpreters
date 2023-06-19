expression -> equality ; 
equality -> comparison ( ( "!=" | "==" ) comparison )* ; 
comparison -> term ( ( ">" | ">=" | "<" | "<=" ) term)* ;
term -> factor ( ( "-" | "+" ) factor )* ;
factor -> unary ( ( "/" | "*" ) unary)*;
unary -> ( "!" | "-" ) unary | primary ;
primary -> literal | grouping ; 
literal -> NUMBER | STRING| "true" | "false" | "nil" ;
grouping -> "(" expression ")" ;