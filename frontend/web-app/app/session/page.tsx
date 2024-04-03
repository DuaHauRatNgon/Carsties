import React from 'react'
import { getSession, getTokenWorkaround } from '../actions/authActions'
import Heading from '../components/Heading';

export default async function Session() {
    const session = await getSession();
    const token = await getTokenWorkaround();

    return (
        <div>
            <Heading title='Session dashboard' />

            <div className=''>
                <h3 className='text-lg'>Session data</h3>
                <pre>{JSON.stringify(session, null, 2)}</pre>
            </div>
            <div className=''>
                <h3 className='text-lg'>Token data</h3>
                <pre className=''>{JSON.stringify(token, null, 2)}</pre>
            </div>
        </div>
    )
}