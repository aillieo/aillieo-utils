#ifndef _V_ARRAY_H
#define _V_ARRAY_H

#include <stdio.h>
#include <tchar.h>
#include <malloc.h>
#include <stdlib.h>
#include <string.h>

typedef struct
{
	int size;
	int* data;
}VArray;

VArray* VArrayInit(int len)
{
	VArray* t = (VArray*)malloc(sizeof(VArray));
	t->size = len;
	t->data = (int*)malloc(len *sizeof(int));

	// init with 0
	memset(t->data,0,len *sizeof(int));
	return t;
}

void VArrayFree(VArray* t)
{
	free(t->data);
	t->data = NULL;
	free(t);
	t = NULL;
}


void VArrayResize(VArray* t, int len)
{
	if(len == t->size)
		return;
	else
	{
		// keep old
		int dataLenToKeep = (len < t->size? len : t->size) * sizeof(int);
		int* tmpData = (int*)malloc(dataLenToKeep);
		memcpy(tmpData, t->data, dataLenToKeep);

		// create new
		free(t->data);
		t->data = (int*)malloc(len *sizeof(int));

		// copy to new
		memcpy(t->data, tmpData, dataLenToKeep);

		// init raw part
		if(len > t->size)
			memset(t->data + t->size , 0, len *sizeof(int) - dataLenToKeep);

		// assign new length
		t->size = len;

		// free temp
		free(tmpData);
	}

}


void VArrayPrint(VArray* t)
{
	int i = 0;
	if(NULL != t)
	{
		printf("[ ");
		for (i = 0 ; i < t->size; i++)
		{
			printf("%d ",t->data[i]);
		}
		printf("]\n");
	}
}


#endif