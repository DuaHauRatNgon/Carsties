'use client'

import React from 'react'
import Countdown, { zeroPad } from 'react-countdown';
import { render } from 'react-dom';

type Props = {
    auctionEnd: string;
}

const renderer = ({ days, hours, minutes, seconds, completed }: 
    {days: number, hours: number, minutes: number, seconds: number, completed: boolean}) => {
        if(completed) { 
            return (
                <div >
                    <span>Auction finished</span>
                </div>
            )
        }
        else {
            return (
                <div >
                    <span suppressHydrationWarning={true}>
                        {zeroPad(days)}:{zeroPad(hours)}:{zeroPad(minutes)}:{zeroPad(seconds)}
                    </span>
                </div>
            )
        }
  };


export default function CountdownTimer({auctionEnd}: Props) {
  return (
    <div>
        <Countdown date={auctionEnd} renderer={renderer} />
    </div>
  )
}