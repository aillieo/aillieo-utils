#include "CStringExt.h"

#include <stdio.h>
#include <tchar.h>
#include <malloc.h>
#include <stdlib.h>
#include <string.h>



int strToHex(const char* src, char* des)
{
	return dataToHex(src,des,sizeof(src));
}


int dataToHex(const char* src, char* des, int length)
{
	int i = 0 ;
	for(i = 0; i < length ; i++ )
	{
		char hex[5];
		sprintf(hex,"\\x%02X",(unsigned char)src[i]);
		memcpy(des + i*4 , hex,4);
	}
	return 0;
}



int bufferToInt(const char* buffer)
{
	int i, j, ret = 0;
	int iExp = 1; 

	for(i = 0 ; i < 4 ; i ++)
	{
		iExp = 1;
		for(j = i ; j < 3 ; j ++ )
		{
			iExp*= 256;
		}
		ret += (buffer[i] & 0x000000ff)  * iExp;
	}
	return ret;
}


short bufferToShort(const char* buffer)
{
	short s = 0;
	s =(((short)buffer[0])<<8)|((short)buffer[1]&0x00FF);
	return s;
}



long long atoll(const char* str)
{
	long long ret;
	int i;

	ret = 0;
	for (; *str; str++) {
		ret = 10*ret + (*str - '0');
	}
	return ret;
}

