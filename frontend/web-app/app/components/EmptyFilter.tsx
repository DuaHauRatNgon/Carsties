'use client'

import { useParamsStore } from '@/hooks/useParamsStore'
import React from 'react'
import { Button } from 'flowbite-react'
import { signIn } from 'next-auth/react'

type Props = {
    showReset?: boolean
    showLogin?: boolean
    callbackUrl?: string
}

export default function EmptyFilter({showReset,showLogin,callbackUrl}: Props) {
    // const reset = useParamsStore(state => state.reset);

    return (
        <div className='h-[40vh] flex flex-col gap-2 justify-center items-center shadow-lg'>
            <div className='mt-4'>
                {showLogin && (
                    <Button outline onClick={() => signIn('id-server', {callbackUrl})}>Login</Button>
                )}
            </div>
        </div>
    )
}