#pragma once

#include <vector>
#include <string>
#include "StringBuilder.h"

class StringVec
{
public:
	StringVec(const std::string& sep = "");
	~StringVec(void);

	void append(const std::string& str);
	void append(const int& i);
	std::string concat() const;

private:

	StringBuilder *sb;
	std::string sep;

	void appendSep();

};
