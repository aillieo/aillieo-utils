#include <vector>

#pragma once

using std::string;

namespace ExtColor
{

	struct Color3B
	{
		unsigned char r;
		unsigned char g;
		unsigned char b;
		Color3B(unsigned char _r, unsigned char _g, unsigned char _b):r(_r),g(_g),b(_b){}
	};

	struct Color4B
	{
		unsigned char r;
		unsigned char g;
		unsigned char b;
		unsigned char a;
		Color4B(unsigned char _r, unsigned char _g, unsigned char _b, unsigned char _a):r(_r),g(_g),b(_b),a(_a){}
	};


	// 16进制转化为RGB
	Color3B Hex2Color3B(string& hex);
	Color4B Hex2Color4B(string& hex);
}
