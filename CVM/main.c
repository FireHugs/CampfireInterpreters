#include "common.h"
#include "chunk.h"
#include "debug.h"
#include "vm.h"

int main()
{
	initVM();

	Chunk chunk;
	initChunk(&chunk);

	int constant = addConstant(&chunk, 1.2);
	writeChunk(&chunk, OP_CONSTANT, 123);
	writeChunk(&chunk, constant, 123);

	writeChunk(&chunk, OP_RETURN, 123);

#ifdef DEBUG_TRACE_EXECUTION
	disassembleChunk(&chunk, "test chunk");
#endif
	interpret(&chunk);
	freeVM();
	freeChunk(&chunk);

	return 0;
}