#include "StringBuilder.h"

using namespace std;

StringBuilder::StringBuilder()
{
	sb = new stringbuf();
	os = new ostream(sb);
}

StringBuilder::~StringBuilder()
{
	delete(os);
	os = nullptr;
	delete(sb);
	sb = nullptr;
}


StringBuilder& StringBuilder::append( const char c)
{
	sb->sputc(c);
	return *this;
}

StringBuilder& StringBuilder::append( const char* c, int n )
{
	sb->sputn(c,n);
	return *this;
}

StringBuilder& StringBuilder::append(const string& str)
{
	sb->sputn(str.c_str(),str.length());
	return *this;
}

StringBuilder& StringBuilder::append(const int& i)
{
	*os << i;
	return *this;
}

StringBuilder& StringBuilder::append( const long& l )
{
	*os << l;
	return *this;
}

StringBuilder& StringBuilder::append( const float& f )
{
	*os << f;
	return *this;
}

StringBuilder& StringBuilder::append( const double& d )
{
	*os << d;
	return *this;
}

string StringBuilder::getString()
{
	return sb->str();
}

void StringBuilder::clear()
{
	sb->str("");
}

streamsize StringBuilder::length()
{
	return sb->in_avail();
}
