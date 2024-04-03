'use client'

import { Button, Dropdown } from 'flowbite-react'
import { User } from 'next-auth'
import { signOut } from 'next-auth/react'
import Link from 'next/link'
import React from 'react'
import { AiFillCar, AiFillTrophy, AiOutlineLogout } from 'react-icons/ai'
import {HiCog, HiUser} from 'react-icons/hi2';

type Props = {
  user: Partial<User>
}

export default function UserActions({user}: Props) {
  return (
    <Dropdown
      inline
      label={`Xin chào ${user.name} !`}
    >
      <Dropdown.Item >
        <Link href='/'>
          My Auctions
        </Link>
      </Dropdown.Item>
      <Dropdown.Item >
        <Link href='/'>
          Auctions đã thắng
        </Link>
      </Dropdown.Item>
      <Dropdown.Item >
        <Link href='/'>
          Bán xe
        </Link>
      </Dropdown.Item>
      <Dropdown.Item >
        <Link href='/session'>
          Session
        </Link>
      </Dropdown.Item>
      <Dropdown.Divider />
      <Dropdown.Item  onClick={() => signOut({callbackUrl: '/'})}>
        Đăng xuất
      </Dropdown.Item>
    </Dropdown>
  )
}