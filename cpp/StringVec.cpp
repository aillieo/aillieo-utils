#include "StringVec.h"
#include <string>
#include <vector>
#include <sstream>

using namespace std;

StringVec::~StringVec(void)
{
	delete(this->sb);
	this->sb = nullptr;
}

StringVec::StringVec(const std::string& sep)
{
	sb = new StringBuilder();
	this->sep = sep;
}

void StringVec::append(const std::string& str)
{
	appendSep();
	sb->append(str);
}

void StringVec::append(const int& i)
{
	appendSep();
	sb->append(i);
}

std::string StringVec::concat() const
{
	return sb->getString();
}

void StringVec::appendSep()
{
	if(sb->length()>0 && sep.length()>0)
	{
		sb->append(sep);
	}
}

