#include "stdlib.h"

#include "CLI.h"

CommandInfo* CommandInfo_New(GString* command, int argsCount, int order)
{
	CommandInfo* this;
	this = (CommandInfo*)malloc(sizeof(CommandInfo));

	this->_command = command;
	this->_argsCount = argsCount;
	this->_order = order;

	this->_args = g_ptr_array_new();
	this->_processList = g_ptr_array_new();

	return this;
}
