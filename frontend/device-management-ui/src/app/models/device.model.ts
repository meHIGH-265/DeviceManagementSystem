export interface Device {
  id?: number;
  name: string;
  manufacturer: string;
  type: string;
  os: string;
  osVersion?: string;
  processor?: string;
  ram?: string;
  description?: string;
  assignedUserId?: number | null;
}
