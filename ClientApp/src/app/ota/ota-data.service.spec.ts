import { TestBed } from '@angular/core/testing';

import { OTADataService } from './ota-data.service';

describe('OTADataService', () => {
  let service: OTADataService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(OTADataService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
