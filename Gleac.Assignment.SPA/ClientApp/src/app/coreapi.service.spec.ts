import { TestBed } from '@angular/core/testing';

import { CoreapiService } from './coreapi.service';

describe('CoreapiService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: CoreapiService = TestBed.get(CoreapiService);
    expect(service).toBeTruthy();
  });
});
