import React from 'react'
import {AiOutlineCar} from 'react-icons/ai';

export default function Navbar() {
  return (
    <header className='
      sticky top-0 z-50 flex justify-between bg-white p-5 items-center text-gray-800 shadow-md
    '>
        <div>Logo</div>
        <div>Search</div>
      <div>Login</div>
    </header>
  )
}