#include "ExtString.h"
#include <string>
#include <stdarg.h>
#include <codecvt>

using std::string;
using std::wstring;

void ExtString::reverseString(string& str)
{
	reverse(str.begin(), str.end());
}

void ExtString::reverseString(wstring& str)
{
	reverse(str.begin(), str.end());
}

string ExtString::getReversedString(const string& str)
{
	return string(str.rbegin(), str.rend());
}

wstring ExtString::getReversedString(const wstring& str)
{
	return wstring(str.rbegin(), str.rend());
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

string ExtString::format( const char * format, ... )
{
	char* pBuf = (char*)malloc(MaxStringLen);
	if(pBuf == nullptr)
	{
		return "";
	}
	else
	{
		va_list arglist;
		va_start(arglist, format);
		vsnprintf(pBuf, MaxStringLen, format, arglist);
		va_end(arglist);
		string tmp = string(pBuf);
		free(pBuf);
		return tmp;
	}
}

vector<string> ExtString::splitString( const string& str , const string& sep )
{
	vector<string> strs;
	if (str.empty())
	{
		return strs;
	}

	string tmp;
	string::size_type pos_begin = str.find_first_not_of(sep);
	string::size_type comma_pos = 0;

	while (pos_begin != string::npos)
	{
		comma_pos = str.find(sep, pos_begin);
		if (comma_pos != string::npos)
		{
			tmp = str.substr(pos_begin, comma_pos - pos_begin);
			pos_begin = comma_pos + sep.length();
		}
		else
		{
			tmp = str.substr(pos_begin);
			pos_begin = comma_pos;
		}

		if (!tmp.empty())
		{
			strs.push_back(tmp);
			tmp.clear();
		}
	}
	return strs;
}

string ExtString::trim( const string& str, bool trimLeft/*=true*/, bool trimRight/*=true*/ )
{
	if(!trimLeft && !trimRight){
		return str;
	}

	string tmp = str;
	string::iterator it;

	if(trimLeft)
	{
		for (it = tmp.begin(); it != tmp.end(); it++) 
		{
			if (!isspace(*it)) 
			{
				tmp.erase(tmp.begin(), it);
				break;
			}
		}

		if (it == tmp.end()) 
		{
			return tmp;
		}
	}

	if (trimRight)
	{
		for (it = tmp.end() - 1; it != tmp.begin(); it--) 
		{
			if (!isspace(*it)) 
			{
				tmp.erase(it + 1, tmp.end());
				break;
			}
		}
	}

	return tmp;
}


wstring ExtString::string2wstring( const string& str )
{
	typedef std::codecvt_utf8<wchar_t> convert_typeX;
	std::wstring_convert<convert_typeX, wchar_t> converterX;
	return converterX.from_bytes(str);
}

string ExtString::wstring2string( const wstring& str )
{
	typedef std::codecvt_utf8<wchar_t> convert_typeX;
	std::wstring_convert<convert_typeX, wchar_t> converterX;
	return converterX.to_bytes(str);
}


vector<string> ExtString::file2vector( const char* filepath )
{
	vector<string> ret = vector<string>();
	if(NULL == filepath)
	{
		return ret;
	}

	// read file to buffer

	unsigned char* pBuffer = NULL;
	unsigned long bufferSize = 0;
	do
	{
		FILE *fp = fopen(filepath, "r");
		if(!fp)
		{
			return ret;
		}
		fseek(fp,0,SEEK_END);
		bufferSize = ftell(fp);
		fseek(fp,0,SEEK_SET);
		pBuffer = new unsigned char[bufferSize];
		bufferSize = fread(pBuffer,sizeof(unsigned char), bufferSize,fp);
		fclose(fp);
	} while (0);

	if (!pBuffer)
	{
		return ret;
	}

	// move from buffer to vector

	int lineLen = 0;
	string lineStr = ""; 
	for (unsigned long i = 0; i < bufferSize; i++)
	{
		if (pBuffer[i] != '\r' && pBuffer[i] != '\n' && pBuffer[i] != '\0')
		{
			lineStr += pBuffer[i];
			lineLen++;
		}
		else
		{
			if (lineLen > 0)
			{
				std::string oneLineStr(lineStr.data(), lineStr.length());
				ret.push_back(oneLineStr);
				lineStr = "";
				lineLen = 0 ;
			}
		}
	}
	delete[] pBuffer;
	return ret;
}

string ExtString::formatDeltaTimestamp( long deltaTime , const char* separator )
{
	long secs = deltaTime < 0 ? 0 : deltaTime;

	if (secs > 24 * 3600) {
		return format("%dD %02d:%02d:%02d",secs / (24 * 3600), secs / 3600 % 24, secs / 60 % 60, secs % 60);
	}
	return format("%02d:%02d:%02d", secs / 3600, secs / 60 % 60, secs % 60);
}


