"use client";

import { useState, useEffect } from "react";
import { useCallback } from "react";

import { TheaterHeader } from "./theater-header";

import { TimeFrameTabs } from "./time-frame-tabs";

import { DateRangeFilter } from "./data-range-filter";

import { MetricsCards } from "./metrics-cards";

import { RevenueChart } from "./revenue-chart";

import { TicketSalesChart } from "./ticket-sales-chart";

import { OccupancyChart } from "./occupancy-chart";

import { MoviePerformanceTable } from "./movie-performance-table";

import { theaterData, refreshData } from "./mock-data";

export default function TheaterAnalyticsDashboard() {
  const [selectedTimeFrame, setSelectedTimeFrame] = useState("daily");
  const [selectedTheater, setSelectedTheater] = useState("qfx");
  const [selectedScreen, setSelectedScreen] = useState("All Screens");
  const [data, setData] = useState(null);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);

  // Function to safely get data
  const getTheaterData = useCallback(() => {
    try {
      // Make sure the theater exists
      if (!theaterData[selectedTheater]) {
        console.error(`Theater '${selectedTheater}' not found in data`);
        return theaterData.qfx.daily["All Screens"]; // Fallback
      }

      // Make sure the time frame exists
      if (!theaterData[selectedTheater][selectedTimeFrame]) {
        console.error(
          `Time frame '${selectedTimeFrame}' not found for theater '${selectedTheater}'`
        );
        return theaterData[selectedTheater].daily["All Screens"]; // Fallback
      }

      // Make sure the screen exists
      if (!theaterData[selectedTheater][selectedTimeFrame][selectedScreen]) {
        console.error(
          `Screen '${selectedScreen}' not found for theater '${selectedTheater}' and time frame '${selectedTimeFrame}'`
        );
        return theaterData[selectedTheater][selectedTimeFrame]["All Screens"]; // Fallback
      }

      // Return the data if all checks pass
      return theaterData[selectedTheater][selectedTimeFrame][selectedScreen];
    } catch (err) {
      console.error("Error getting theater data:", err);
      return theaterData.qfx.daily["All Screens"]; // Fallback to default data
    }
  }, [selectedTheater, selectedTimeFrame, selectedScreen]);

  // Update data when selections change
  useEffect(() => {
    try {
      setIsLoading(true);
      setError(null);

      // Simulate network delay
      setTimeout(() => {
        const newData = getTheaterData();
        setData(newData);
        setIsLoading(false);
      }, 300);
    } catch (err) {
      console.error("Error in useEffect:", err);
      setError("Failed to load data. Please try again.");
      setIsLoading(false);
    }
  }, [selectedTimeFrame, selectedTheater, selectedScreen, getTheaterData]);

  // Handle refresh button click
  const handleRefresh = useCallback(() => {
    if (!data) return;

    setIsLoading(true);
    setError(null);

    // Simulate API call with a delay
    setTimeout(() => {
      try {
        // Generate refreshed data with slight variations
        const refreshedData = refreshData(data);
        setData(refreshedData);
      } catch (err) {
        console.error("Error refreshing data:", err);
        setError("Failed to refresh data. Please try again.");
      } finally {
        setIsLoading(false);
      }
    }, 800); // Simulate network delay
  }, [data]);

  // Handle theater change
  const handleTheaterChange = useCallback((theater) => {
    console.log(`Changing theater to: ${theater}`);
    setSelectedTheater(theater);
  }, []);

  // Handle time frame change
  const handleTimeFrameChange = useCallback((timeFrame) => {
    console.log(`Changing time frame to: ${timeFrame}`);
    setSelectedTimeFrame(timeFrame);
  }, []);

  // Handle screen change
  const handleScreenChange = useCallback((screen) => {
    console.log(`Changing screen to: ${screen}`);
    setSelectedScreen(screen);
  }, []);

  // Show loading state if data is not yet loaded
  if (!data && isLoading) {
    return (
      <div className="container mx-auto p-4 max-w-7xl">
        <div className="flex justify-center items-center h-96">
          <div className="text-center">
            <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-primary mx-auto mb-4"></div>
            <p>Loading data...</p>
          </div>
        </div>
      </div>
    );
  }

  // Show error state if there was an error
  if (error) {
    return (
      <div className="container mx-auto p-4 max-w-7xl">
        <div className="flex justify-center items-center h-96">
          <div className="text-center text-red-500">
            <p>{error}</p>
            <button
              onClick={() => window.location.reload()}
              className="mt-4 px-4 py-2 bg-primary text-white rounded-md"
            >
              Reload Page
            </button>
          </div>
        </div>
      </div>
    );
  }

  // Use default data if data is still null
  const displayData = data || theaterData.qfx.daily["All Screens"];

  return (
    <div className="container mx-auto p-4 max-w-7xl">
      <TheaterHeader
        onRefresh={handleRefresh}
        onTheaterChange={handleTheaterChange}
        selectedTheater={selectedTheater}
        isLoading={isLoading}
      />

      <div className="mb-6">
        <h2 className="text-2xl font-bold mb-4">Theater Analytics</h2>
        <div className="flex justify-between items-center mb-4">
          <TimeFrameTabs
            selectedTimeFrame={selectedTimeFrame}
            onTimeFrameChange={handleTimeFrameChange}
          />
          <DateRangeFilter
            selectedScreen={selectedScreen}
            onScreenChange={handleScreenChange}
            dateRange={displayData.dateRange}
          />
        </div>

        <div
          className={`transition-opacity duration-300 ${
            isLoading ? "opacity-50" : "opacity-100"
          }`}
        >
          <MetricsCards metrics={displayData.metrics} />

          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6 mb-8">
            <RevenueChart
              data={displayData.revenueData}
              title={`${
                selectedTimeFrame.charAt(0).toUpperCase() +
                selectedTimeFrame.slice(1)
              } Revenue Trend`}
            />
            <TicketSalesChart
              data={displayData.ticketData}
              title={`${
                selectedTimeFrame.charAt(0).toUpperCase() +
                selectedTimeFrame.slice(1)
              } Ticket Sales`}
            />
            <OccupancyChart
              data={displayData.occupancyData}
              title="Average Occupancy"
            />
          </div>

          <MoviePerformanceTable data={displayData.movieData} />
        </div>
      </div>
    </div>
  );
}
