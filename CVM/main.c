#include "common.h"
#include "chunk.h"
#include "debug.h"
#include "vm.h"

void pushConstant(Chunk* chunk);

int main()
{
	initVM();

	Chunk chunk;
	initChunk(&chunk);

	pushConstant(&chunk, 1.2);	
	
	pushConstant(&chunk, 3.4);

	writeChunk(&chunk, OP_ADD, 123);

	pushConstant(&chunk, 5.6);

	writeChunk(&chunk, OP_DIVIDE, 123);
	writeChunk(&chunk, OP_NEGATE, 123);

	writeChunk(&chunk, OP_RETURN, 123);

#ifdef DEBUG_TRACE_EXECUTION
	disassembleChunk(&chunk, "test chunk");
#endif
	interpret(&chunk);
	freeVM();
	freeChunk(&chunk);

	return 0;
}

void pushConstant(Chunk* chunk, Value value)
{
	int constant = addConstant(chunk, value);
	writeChunk(chunk, OP_CONSTANT, 123);
	writeChunk(chunk, constant, 123);
}