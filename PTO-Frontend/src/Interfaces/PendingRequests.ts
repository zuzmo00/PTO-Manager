import type PendingRequestBlockDto from "./PendingRequestBlockDto";
import type RequestsGetDto from "./RequestsGetDto.ts";

export default interface PendingRequests {
    pendingRequestBlock:PendingRequestBlockDto
    requests:RequestsGetDto[]
}