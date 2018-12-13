#include "stdio.h"

#include "glib.h"
#include "gmodule.h"

typedef struct _commandInfo
{
	GString* _command;
	int _argsCount;
	int _order;

	GPtrArray* _args;
	GPtrArray* _processList;
} CommandInfo;

CommandInfo* CommandInfo_New(GString* command, int argsCount, int order);

GString* CommandInfo_GetCommand();
void CommandInfo_SetCommand(GString* value);

int CommandInfo_GetArgsCount();
void CommandInfo_SetArgsCount(int value);

int CommandInfo_GetOrder();
void CommandInfo_SetOrder(int value);
