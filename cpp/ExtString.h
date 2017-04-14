#include <string>
#include <vector>

#pragma once

using std::string;

namespace ExtString 
{
	void reverseString(string& str);

	std::string getReversedString(const string& str);

	char* reverseCString(char* cstr);
	
}
