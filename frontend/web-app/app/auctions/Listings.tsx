'use client'

import React, { useEffect, useState } from 'react'
import AuctionCard from './AuctionCard';
import { Auction, PagedResult } from '../types';
import AppPagination from '../components/AppPagination';
import { getData } from '../actions/auctionActions';

export default function Listings() {
    const [auction, setAuctions] = useState<Auction[]>([]);
    const [pageCount, setPageCount] = useState(0);
    const [pageNumber, setPageNumber] = useState(1);


    useEffect (() => {
        getData(pageNumber).then(data => {
            setAuctions(data.results); 
            setPageCount(data.pageCount);
        })
    }, [pageNumber])
    
    return (
        <>
            <div className='grid grid-cols-4 gap-6'>
                {auction.map(auction => (
                    <AuctionCard auction={auction} key={auction.id} />
                ))}
            </div>
            <div className='flex justify-center mt-4'>
                <AppPagination pageChanged={setPageNumber} currentPage={pageNumber} pageCount={pageCount} />
            </div>
        </>
        
    )
}