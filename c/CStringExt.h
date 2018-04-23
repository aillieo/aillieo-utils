#ifndef _V_CSTRING_EXT_H_
#define _V_CSTRING_EXT_H_


int strToHex(const char* src, char* des);

int dataToHex(const char* src, char* des, int length);

int bufferToInt(const char* buffer);

short bufferToShort(const char* buffer);

long long atoll(const char* instr);

#endif