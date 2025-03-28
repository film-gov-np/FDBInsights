"use client";

import { Button } from "@/components/ui/button";
import { RefreshCw } from "lucide-react";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";

export function TheaterHeader({
  onRefresh,
  onTheaterChange,
  selectedTheater,
  isLoading,
}) {
  return (
    <div className="flex justify-between items-center mb-6">
      <h1 className="text-xl font-bold">Nepali Film Development Board</h1>
      <div className="flex items-center gap-2">
        <Button
          variant="outline"
          size="sm"
          className="flex items-center gap-1"
          onClick={onRefresh}
          disabled={isLoading}
        >
          <RefreshCw className={`h-4 w-4 ${isLoading ? "animate-spin" : ""}`} />
          <span>{isLoading ? "Refreshing..." : "Refresh Data"}</span>
        </Button>
        <Select value={selectedTheater} onValueChange={onTheaterChange}>
          <SelectTrigger className="w-[180px]">
            <SelectValue placeholder="Select Theater" />
          </SelectTrigger>
          <SelectContent>
            <SelectItem value="qfx">Theater: QFX Cinemas</SelectItem>
            <SelectItem value="fcube">Theater: FCube Cinemas</SelectItem>
            <SelectItem value="big">Theater: Big Movies</SelectItem>
          </SelectContent>
        </Select>
      </div>
    </div>
  );
}
