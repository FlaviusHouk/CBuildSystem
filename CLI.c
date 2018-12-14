#include "stdlib.h"

#include "CLI.h"

CommandInfo* CommandInfo_New(GString* command, int argsCount, int order, void (*action)(CommandInfo* com))
{
	CommandInfo* this;
	this = (CommandInfo*)malloc(sizeof(CommandInfo));

	this->_command = command;
	this->_argsCount = argsCount;
	this->_order = order;

	this->_args = g_ptr_array_new();
	this->_action = action;

	return this;
}

GString* CommandInfo_GetCommand(CommandInfo* this)
{
	return this->_command;
}

void CommandInfo_SetCommand(CommandInfo* this, GString* value)
{
	this->_command = value;
}

int CommandInfo_GetArgsCount(CommandInfo* this)
{
	return this->_argsCount;
}

void CommandInfo_SetArgsCount(CommandInfo* this, int value)
{
	this->_argsCount = value;
}

int CommandInfo_GetOrder(CommandInfo* this)
{
	return this->_order;
}

void CommandInfo_SetOrder(CommandInfo* this, int value)
{
	this->_order = value;
}

GPtrArray* CommandInfo_GetArgs(CommandInfo* this)
{
	return this->_args;
}

void CommandInfo_ProcessCommand(CommandInfo* this)
{
	if(this->_action != NULL)
		this->_action(this);
}

static GPtrArray* AvailableCommands;

static void ProcessCreateCommand(CommandInfo* command)
{}

static void InitializeStatics()
{
	AvailableCommands = g_ptr_array_new();

	g_ptr_array_add(AvailableCommands, 
					CommandInfo_New(g_string_new("--create"),
									1,
									1,
								    ProcessCreateCommand));
}
