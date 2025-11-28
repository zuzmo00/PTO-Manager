import type statDetailGetDto from "./statDetailGetDto";

export default interface DayOutStatisticsDto{
    name: string,
    value: number,
    color: string,
    details: statDetailGetDto
}
