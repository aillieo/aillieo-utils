local function SplitString(str, sep)
    if type(str) ~= "string" then return end
    if type(sep) ~= "string" then return end
    if (sep == '') then return {str} end
    local ret, pos = {}, 0
    for st,sp in function() return string.find(str, sep, pos, true) end do
        table.insert(ret, string.sub(str, pos, st - 1))
        pos = sp + 1
    end
    table.insert(ret, string.sub(str, pos))
    return ret
end


return {
    SplitString = SplitString
}