#pragma once

#include <iostream>

namespace NvencPlugin
{
    class ITexture2D
    {
    public:
        ITexture2D(uint32_t w, uint32_t h) : m_width(w), m_height(h) {}
        bool IsSize(uint32_t w, uint32_t h) const { return m_width == w && m_height == h; }

        virtual ~ITexture2D() = 0;

        virtual void* GetNativeTexturePtrV() = 0;
        virtual const void* GetNativeTexturePtrV() const = 0;

        virtual void* GetEncodeTexturePtrV() = 0;
        virtual const void* GetEncodeTexturePtrV() const = 0;

        virtual void* GetNV12Texture() = 0;
        virtual const void* GetNV12Texture() const = 0;

        uint32_t GetWidth() const { return m_width; };
        uint32_t GetHeight()  const { return m_height; };

    protected:
        uint32_t m_width;
        uint32_t m_height;
    };
}
