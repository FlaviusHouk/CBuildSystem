#include "stdio.h"
#include "stdlib.h"

#include "CLI.h"

int main()
{
	CommandInfo* command = CommandInfo_New(g_string_new("Command"), 1, 1);
	printf("Done");
}
