export interface ModelAdapter<T> {
    adapt(item: any): T;
  }