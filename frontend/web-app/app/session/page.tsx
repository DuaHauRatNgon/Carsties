import React from 'react'
import { getSession } from '../actions/authActions'
import Heading from '../components/Heading';

export default async function Session() {
    const session = await getSession();

    return (
        <div>
            <Heading title='thông tin session' />
            <div className=''>
                <h3 className='text-lg'>Session data</h3>
                <pre>{JSON.stringify(session, null, 2)}</pre>
            </div>
            {/* <div className='mt-4'>
                <AuthTest />
            </div> */}
            <div className=''>
                <h3 className='text-lg'>Token data</h3>
                {/* <pre className='overflow-auto'>{JSON.stringify(token, null, 2)}</pre> */}
            </div>
        </div>
    )
}