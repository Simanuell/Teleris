using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.ComponentModel;

namespace Teleris
{
    public class EngineTimer
    {

     // Windows CE native library with QueryPerformanceCounter().
     private const string lib = "Kernel32.dll";
     [DllImport(lib)]
     private static extern bool QueryPerformanceCounter(out long count);
     [DllImport(lib)]
     private static extern bool QueryPerformanceFrequency(out long frequency);


    public EngineTimer() 
    {

    mSecondsPerCount = 0.0; 
    mDeltaTime= -1.0; 
    mBaseTime = 0; 
    mPausedTime = 0;    
    mPrevTime = 0; 
    mCurrTime = 0; 
    mStopped = false;

    Int64 countsPerSec;
    QueryPerformanceFrequency(out countsPerSec);
    mSecondsPerCount = 1.0 / (double)countsPerSec;

    }

    public float TotalTime() 
    {

        if (mStopped)
        {

            return (float)(((mStopTime - mPausedTime) - mBaseTime) * mSecondsPerCount);
        }

        else
        {
            return (float)(((mCurrTime - mPausedTime) - mBaseTime) * mSecondsPerCount);      
        }

    }  // in seconds

    public float DeltaTime() 
    {
        //System.Diagnostics.Trace.WriteLine(mDeltaTime);
        return (float)mDeltaTime;
    
    } // in seconds

    // Call before message loop.
    public void Reset() 
    {

    Int64 currTime = 0;

    QueryPerformanceCounter(out currTime);
    //System.Console.WriteLine(currTime.ToString());
    mBaseTime = currTime;
    mPrevTime = currTime;
    mStopTime = 0;
    mStopped = false;

    }
    // Call when unpaused.
    public void Start() 
    {

        Int64 startTime = 0;
        QueryPerformanceCounter(out startTime);
        //System.Console.WriteLine(startTime.ToString());
        // Accumulate the time elapsed between stop and start pairs.
        //
        //                     |<-------d------->|
        // ----*---------------*-----------------*------------> time
        //  mBaseTime       mStopTime        startTime     

        if (mStopped)
        {
            mPausedTime += (startTime - mStopTime);

            mPrevTime = startTime;
            mStopTime = 0;
            mStopped = false;
        }
    
    
    }
    // Call when paused.
    public void Stop() 
    {

        if (!mStopped)
        {
            Int64 currTime = 0;
            QueryPerformanceCounter(out currTime);

            mStopTime = currTime;
            mStopped = true;
        }
    
    
    
    }
    // Call every frame.
    public void Tick() 
    {

        if (mStopped)
        {
            mDeltaTime = 0.0;
            
            return;
        }

        Int64 currTime = 0;
        QueryPerformanceCounter(out currTime);
        mCurrTime = currTime;
        

        // Time difference between this frame and the previous.
        mDeltaTime = (mCurrTime - mPrevTime) * mSecondsPerCount;
  
        // Prepare for next frame.
        mPrevTime = mCurrTime;
        
        // Force nonnegative.  The DXSDK's CDXUTTimer mentions that if the 
        // processor goes into a power save mode or we get shuffled to another
        // processor, then mDeltaTime can be negative.
        if (mDeltaTime < 0.0)
        {
            mDeltaTime = 0.0;
        }
    
    
    }  


	private double mSecondsPerCount;
	private double mDeltaTime;
    

	private Int64 mBaseTime;
	private Int64 mPausedTime;
	private Int64 mStopTime;
	private Int64 mPrevTime;
    private Int64 mCurrTime;

	private bool mStopped;
        
    }


}

