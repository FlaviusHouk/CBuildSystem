#include "stdio.h"

#include "glib.h"
#include "gmodule.h"

typedef struct _commandInfo
{
	GString* _command;
	int _argsCount;
	int _order;

	GPtrArray* _args;
	void (*_action)(struct _commandInfo* com);
} CommandInfo;

CommandInfo* CommandInfo_New(GString* command, int argsCount, int order, void (*action)(CommandInfo* com));

GString* CommandInfo_GetCommand(CommandInfo* this);
void CommandInfo_SetCommand(CommandInfo* this, GString* value);

int CommandInfo_GetArgsCount(CommandInfo* this);
void CommandInfo_SetArgsCount(CommandInfo* this, int value);

int CommandInfo_GetOrder(CommandInfo* this);
void CommandInfo_SetOrder(CommandInfo* this, int value);

GPtrArray* CommandInfo_GetArgs(CommandInfo* this);
void CommandInfo_ProcessCommand(CommandInfo* this);

