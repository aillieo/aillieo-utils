#pragma once

#include <string>
#include <iostream>     // std::ostream
#include <sstream>      // std::stringbuf

class StringBuilder
{
public:
	StringBuilder();
	~StringBuilder();
	
	StringBuilder& append(const char c);
	StringBuilder& append(const char* c, int n);
	StringBuilder& append(const std::string& str);
	StringBuilder& append(const int& i);
	StringBuilder& append(const long& l);
	StringBuilder& append(const float& f);
	StringBuilder& append(const double& d);

	std::string getString();

	std::streamsize length();
	void clear();

private:

	std::stringbuf * sb;
	std::ostream * os;
};

