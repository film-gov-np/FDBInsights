"use client";

import { Tabs, TabsList, TabsTrigger } from "@/components/ui/tabs";

export function TimeFrameTabs({ selectedTimeFrame, onTimeFrameChange }) {
  return (
    <Tabs value={selectedTimeFrame} onValueChange={onTimeFrameChange}>
      <TabsList>
        <TabsTrigger value="daily">Daily</TabsTrigger>
        <TabsTrigger value="weekend">Weekend</TabsTrigger>
        <TabsTrigger value="weekly">Weekly</TabsTrigger>
        <TabsTrigger value="monthly">Monthly</TabsTrigger>
      </TabsList>
    </Tabs>
  );
}
