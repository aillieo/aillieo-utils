#include "ExtString.h"
#include <string>

using std::string;

void ExtString::reverseString(string& str)
{
	reverse(str.begin(), str.end());
}

std::string ExtString::getReversedString(const string& str)
{
	return string(str.rbegin(), str.rend());
}

char* ExtString::reverseCString( char* cstr )
{
	size_t len = strlen(cstr);
	size_t i, n = len / 2;
	char c;
	for (i = 0; i <= n; i++)
	{
		c = cstr[i];
		cstr[i] = cstr[len - 1 - i];
		cstr[len - 1 - i] = c;
	}
	return cstr;
}

