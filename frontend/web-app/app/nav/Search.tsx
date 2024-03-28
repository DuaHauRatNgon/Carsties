'use client'

import { useParamsStore } from '@/hooks/useParamsStore';
import React, { useState } from 'react'
import { FaSearch } from 'react-icons/fa';

export default function Search() {
    const setParams = useParamsStore(state => state.setParams);
    const setSearchValue = useParamsStore(state => state.setSearchValue);
    const searchValue = useParamsStore(state => state.searchValue);

    function onChange(event: any) {
        setSearchValue(event.target.value);
    }

    function search() {
        setParams({searchTerm: searchValue});
    }

    return (
        <div className='flex w-[50%] items-center border'>
            <input
                onKeyDown={(e: any) => {
                    if (e.key === 'Enter') search();
                }}
                value={searchValue}
                onChange={onChange}
                type="text"
                placeholder='Tìm hãng xe, model, màu sắc,...'
                className='flex-grow'
            />
            <button onClick={search}>
                <FaSearch
                    size={20}
                    className=' cursor-pointer ' />
            </button>
        </div>
    )
}