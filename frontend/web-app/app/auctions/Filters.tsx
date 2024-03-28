
import { useParamsStore } from '@/hooks/useParamsStore';
import { Button } from 'flowbite-react';
import ButtonGroup from 'flowbite-react/lib/esm/components/Button/ButtonGroup'; 
import React from 'react'

const pageSizeButtons = [4, 8, 12];

export default function Filters() {
    const pageSize = useParamsStore(state => state.pageSize);
    const setParams = useParamsStore(state => state.setParams);

    return (
        <div className='flex justify-between items-center mb-4'>
            <div>
                <span className='uppercase text-sm text-gray-500 mr-2'>Page size</span>
                <Button.Group>
                    {pageSizeButtons.map((value, i) => (
                        <Button key={i}
                        onClick={() => setParams({pageSize : value})}
                        color={`${pageSize === value ? 'red' : 'gray'}`}
                        className=''
                        >
                        {value}
                        </Button>
                    ))}
                </Button.Group>
            </div>
        </div>
    )
}