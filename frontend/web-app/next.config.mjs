/** @type {import('next').NextConfig} */
const nextConfig = {
    experimental: {
        serverActions: true
    },
    images: {
        domains: [
            'img1.oto.com.vn',
            'cdn.pixabay.com'
        ]
    }
};

export default nextConfig;
