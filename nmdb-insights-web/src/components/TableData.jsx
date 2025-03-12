import React from "react";
import {
  Table,
  TableBody,
  TableCaption,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";

const TableData = () => {
  return (
    <div className="mt-10 ">
      <Table>
        <TableCaption className="text-md">Box Office of 2025</TableCaption>
        <TableHeader>
          <TableRow>
            <TableHead className="w-[100px]">Rank</TableHead>
            <TableHead>Movie</TableHead>
            <TableHead>Domestic</TableHead>
            <TableHead className="text-right">%</TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          <TableRow>
            <TableCell className="font-medium">1</TableCell>
            <TableCell>Movie A</TableCell>
            <TableCell>Rs.123456</TableCell>
            <TableCell className="text-right">35.45</TableCell>
          </TableRow>

          <TableRow>
            <TableCell className="font-medium">2</TableCell>
            <TableCell>Movie B</TableCell>
            <TableCell>Rs.12302</TableCell>
            <TableCell className="text-right">5.45</TableCell>
          </TableRow>

          <TableRow>
            <TableCell className="font-medium">2</TableCell>
            <TableCell>Movie 2</TableCell>
            <TableCell>Rs.7894</TableCell>
            <TableCell className="text-right">42.025</TableCell>
          </TableRow>
        </TableBody>
      </Table>
    </div>
  );
};

export default TableData;
