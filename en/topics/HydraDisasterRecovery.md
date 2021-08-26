# Disaster recovery

When disaster recovery of the [S\#.Data](Hydra.md) operation it should be borne in mind that the settings are stored in the [database](StoragesDatabase.md) and the usual deletion of the folders with history may not lead to the desired effect\!

If, after the "failure", the database was deleted and therefore the instruments were deleted, it is possible to restore the instruments from the downloaded files by using the [Synchronization](HydraSynchronizeData.md) operation.
