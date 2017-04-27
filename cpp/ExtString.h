#include <string>
#include <vector>

#pragma once

#define MaxStringLen 1024

using std::string;
using std::wstring;
using std::vector;

namespace ExtString 
{
	// 反转字符串
	void reverseString(string& str);
	void reverseString(wstring& str);

	// 反转字符串
	string getReversedString(const string& str);
	wstring getReversedString(const wstring& str);

	// 反转字符串
	char* reverseCString(char* cstr);

	// 格式化构造字符串
	string format (const char * format, ...);
	
	// 分裂字符串
	vector<string> splitString( const string& str , const string& sep );

	// 修剪字符串
	string trim(const string& str, bool trimLeft=true, bool trimRight=true);

	// string 和 wstring互转
	//wstring string2wstring(const string& str);
	//string wstring2string(const wstring& str);

	// 读取文件 每行一个string
	vector<string> file2vector(const char* filepath);

	string formatDeltaTimestamp(long deltaTime , const char* separator = ":");


}
