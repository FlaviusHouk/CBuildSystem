$INCLUDE = pkg-config --cflags glib-2.0
$LIBS = pkg-config --libs glib-2.0
$cc = gcc

Main: main.o CLI.o
	cc main.o CLI.o `pkg-config --libs glib-2.0` -o cbs.lef

main.o:
	cc -c  main.c -o main.o `pkg-config --cflags glib-2.0`

CLI.o:
	cc -c  CLI.c -o CLI.o `pkg-config --cflags glib-2.0`
