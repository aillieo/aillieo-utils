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

#ifdef WIN32

	std::locale sys_locale("");

	const char* data_from = str.c_str();
	const char* data_from_end = str.c_str() + str.size();
	const char* data_from_next = 0;

	wchar_t* data_to = new wchar_t[str.size() + 1];
	wchar_t* data_to_end = data_to + str.size() + 1;
	wchar_t* data_to_next = 0;

	wmemset( data_to, 0, str.size() + 1 );

	typedef std::codecvt<wchar_t, char, mbstate_t> convert_facet;
	mbstate_t in_state = 0;
	auto result = std::use_facet<convert_facet>(sys_locale).in(
		in_state, data_from, data_from_end, data_from_next,
		data_to, data_to_end, data_to_next );
	if( result == convert_facet::ok )
	{
		std::wstring dst = data_to;
		delete[] data_to;
		return dst;
	}
	else
	{
		printf( "convert error!\n" );
		delete[] data_to;
		return std::wstring(L"");
	}

#else

	typedef std::codecvt_utf8<wchar_t> convert_typeX;
	std::wstring_convert<convert_typeX, wchar_t> converterX;
	return converterX.from_bytes(str);

#endif


}

string ExtString::wstring2string( const wstring& str )
{

#ifdef WIN32

	std::locale sys_locale("");

	const wchar_t* data_from = str.c_str();
	const wchar_t* data_from_end = str.c_str() + str.size();
	const wchar_t* data_from_next = 0;

	int wchar_size = 4;
	char* data_to = new char[(str.size() + 1) * wchar_size];
	char* data_to_end = data_to + (str.size() + 1) * wchar_size;
	char* data_to_next = 0;

	memset( data_to, 0, (str.size() + 1) * wchar_size );

	typedef std::codecvt<wchar_t, char, mbstate_t> convert_facet;
	mbstate_t out_state = 0;
	auto result = std::use_facet<convert_facet>(sys_locale).out(
		out_state, data_from, data_from_end, data_from_next,
		data_to, data_to_end, data_to_next );
	if( result == convert_facet::ok )
	{
		std::string dst = data_to;
		delete[] data_to;
		return dst;
	}
	else
	{
		printf( "convert error!\n" );
		delete[] data_to;
		return std::string("");
	}

#else

	typedef std::codecvt_utf8<wchar_t> convert_typeX;
	std::wstring_convert<convert_typeX, wchar_t> converterX;
	return converterX.to_bytes(str);

#endif

}


string ExtString::wstring2utf8string( const std::wstring& str )
{
	std::wstring_convert<std::codecvt_utf8<wchar_t>> conv;
	return conv.to_bytes( str );
}

wstring ExtString::utf8string2wstring( const std::string& str )
{
	std::wstring_convert<std::codecvt_utf8<wchar_t> > conv;
	return conv.from_bytes( str );
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

size_t ExtString::getUtf8stringLength( const string& str )
{
	auto charLen = [&](const char* chars)
	{
		int x = 0xf0 & (*chars);
		size_t ret;
		x >>= 4;
		ret = 4;
		if (x <= 14)
			ret = 3;
		if (x <= 12)
			ret = 2;
		if (x <= 7)
			ret = 1;
		return ret;
	};


	size_t ret = 0;
	const char* cstr = str.c_str();
	while (cstr && *cstr) {
		cstr += charLen(cstr);
		ret++;
	}
	return ret;

}

string ExtString::timeStampToLocalDate(time_t &t)
{
	struct tm tm;
	char s[100];
	tm = *localtime(&t);
	strftime(s, sizeof(s), "%Y-%m-%d", &tm);
	return s;
}

string ExtString::timeStampToLocalTime(time_t &t)
{
	struct tm tm;
	char s[100];
	tm = *localtime(&t);
	strftime(s, sizeof(s), "%Y-%m-%d  %H:%M:%S", &tm);
	return s;
}

string ExtString::timeStampToUTCData(time_t &t)
{
	struct tm tm;
	char s[100];
	tm = *gmtime(&t);
	strftime(s, sizeof(s), "%Y-%m-%d", &tm);
	return s;
}

string ExtString::timeStampToUTCTime(time_t &t)
{
	struct tm tm;
	char s[100];
	tm = *gmtime(&t);
	strftime(s, sizeof(s), "%Y-%m-%d  %H:%M:%S", &tm);
	return s;
}

string ExtString::formatQuantityShorten( long& val )
{
	long _val = val;
	if (val < 0) 
	{
		_val = -val;
	}
	std::string ret = format("%ld",_val);
	int pos = ret.length() - 3;
	while (pos > 0) {
		ret.insert(pos, ",");
		pos -= 3;
	}
	if (val < 0) {
		ret = "-" + ret;
	}
	return ret;
}

string ExtString::formatQuantityComma( long& val )
{
	char q[7] = { ' ', 'K', 'M', 'G', 'T', 'P', 'E' };
	double _val = val;
	if(val < 0)
	{
		_val = -val;
	}
	if (_val >= 1000) 
	{
		size_t i = 0;
		while (_val / 1000 >= 1) 
		{
			_val = _val / 1000;
			i++;
		}
		string ret = format("%.1f%c", _val, q[i]);
		if(val<0)
		{
			ret = "-" + ret;
		}
		return ret;
	} else 
	{
		return format("%ld",val);
	}
}
