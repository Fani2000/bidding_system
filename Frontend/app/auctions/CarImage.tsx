'use client';

import React, { useState, useMemo } from 'react';
import Image from 'next/image';

type Props = {
  imageUrl: string;
};

export default function CarImage({ imageUrl }: Props) {
  const [isLoading, setLoading] = useState(true);

  const validImageUrl = useMemo(() => {
    if (imageUrl && imageUrl.trim().toLowerCase().startsWith('http')) {
      return imageUrl;
    }
    return null;
  }, [imageUrl]);

  if (!validImageUrl) {
    return (
      <div className="flex items-center justify-center bg-gray-100 text-gray-500 w-full h-full">
        No Image Available
      </div>
    );
  }

  return (
    <Image
      src={validImageUrl}
      fill
      alt="image of car"
      priority
      className={`
        object-cover group-hover:opacity-75 duration-700 ease-in-out
        ${isLoading ? 'grayscale blur-2xl scale-110' : 'grayscale-0 blur-0 scale-100'}
      `}
      sizes="(max-width: 768px) 100vw, (max-width: 1200px) 50vw, 25vw"
      onLoad={() => setLoading(false)}
    />
  );
}
