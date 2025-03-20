import { Button } from "@/components/ui/button";
import { Calendar, Filter } from "lucide-react";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuCheckboxItem,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";

export function DateRangeFilter({ selectedScreen, onScreenChange, dateRange }) {
  // List of available screens
  const screens = [
    "All Screens",
    "Screen 1",
    "Screen 2",
    "Screen 3",
    "Screen 4",
  ];

  return (
    <div className="flex items-center gap-2">
      <Button variant="outline" size="sm" className="flex items-center gap-1">
        <Calendar className="h-4 w-4" />
        <span>{dateRange}</span>
      </Button>
      <DropdownMenu>
        <DropdownMenuTrigger asChild>
          <Button
            variant="outline"
            size="sm"
            className="flex items-center gap-1"
          >
            <Filter className="h-4 w-4" />
            <span>Filters</span>
          </Button>
        </DropdownMenuTrigger>
        <DropdownMenuContent align="end" className="w-56">
          <div className="px-2 py-1.5 text-sm font-semibold">Screen</div>
          {screens.map((screen) => (
            <DropdownMenuCheckboxItem
              key={screen}
              checked={selectedScreen === screen}
              onCheckedChange={() => onScreenChange(screen)}
            >
              {screen}
            </DropdownMenuCheckboxItem>
          ))}
        </DropdownMenuContent>
      </DropdownMenu>
    </div>
  );
}
